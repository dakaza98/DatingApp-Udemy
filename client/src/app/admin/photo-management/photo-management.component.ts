import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css'],
})
export class PhotoManagementComponent implements OnInit {
  nonApprovedPhotos: Photo[] = [];
  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadNonApprovedPhotos();
  }

  loadNonApprovedPhotos() {
    this.adminService
      .getPhotosForApproval()
      .subscribe({ next: (photos) => (this.nonApprovedPhotos = photos) });
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: (_) => this.removePhoto(photoId),
    });
  }

  rejectPhoto(photoId: number) {
    this.adminService
      .rejectPhoto(photoId)
      .subscribe({ next: (_) => this.removePhoto(photoId) });
  }

  private removePhoto(photoId: number) {
    this.nonApprovedPhotos = this.nonApprovedPhotos.filter(
      (photo) => photo.id !== photoId
    );
  }
}
