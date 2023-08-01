using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _uow;

    public AdminController(UserManager<AppUser> userManager, IUnitOfWork uow)
    {
        _uow = uow;
        _userManager = userManager;
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await _userManager.Users
            .OrderBy(u => u.UserName)
            .Select(
                u =>
                    new
                    {
                        u.Id,
                        Username = u.UserName,
                        Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                    }
            )
            .ToListAsync();

        return Ok(users);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        if (string.IsNullOrEmpty(roles))
            return BadRequest("You must select at least one role");

        var selectedRoles = roles.Split(",").ToArray();

        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded)
            return BadRequest("Failed to add to roles");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded)
            return BadRequest("Failed to remove from roles");

        return Ok(await _userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public async Task<ActionResult<PhotoForApprovalDTO>> GetPhotosForModeration()
    {
        return Ok(await _uow.PhotoRepository.GetUnapprovedPhotos());
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpPut("approve-photo/{photoId}")]
    public async Task<ActionResult<PhotoForApprovalDTO>> ApprovePhoto(int photoId)
    {
        var photo = await _uow.PhotoRepository.GetPhotoById(photoId);
        var user = await _uow.UserRepository.GetUserFromPhoto(photo);

        if (photo == null)
            return NotFound();

        if (!user.Photos.AsQueryable().IgnoreQueryFilters().Where(photo => photo.IsMain).Any())
            photo.IsMain = true;

        photo.IsApproved = true;

        if (await _uow.Complete())
            return NoContent();

        return BadRequest("Failed to approve photo");
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpDelete("reject-photo/{photoId}")]
    public async Task<ActionResult<PhotoForApprovalDTO>> RejectPhoto(int photoId)
    {
        var photo = await _uow.PhotoRepository.GetPhotoById(photoId);

        if (photo == null)
            return NotFound();

        _uow.PhotoRepository.RemovePhoto(photo);

        if (await _uow.Complete())
            return NoContent();

        return BadRequest("Faild to reject photo");
    }
}
