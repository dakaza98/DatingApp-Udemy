import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  UntypedFormBuilder,
  FormControl,
  UntypedFormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: UntypedFormGroup = new UntypedFormGroup({});
  maxDate: Date = new Date();

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit(): void {
    this.initilizeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initilizeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });

    this.registerForm.controls.password.valueChanges.subscribe({
      next: () =>
        this.registerForm.controls['confirmPassword'].updateValueAndValidity(),
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo).value
        ? null
        : { notMatching: true };
    };
  }

  register() {
    console.log(this.registerForm?.value);

    // this.accountService.register(this.model).subscribe(
    //   (response) => {
    //     console.log(response);
    //     this.cancel();
    //   },
    //   (error) => {
    //     console.log(error);
    //     this.toastr.error(error.error);
    //   }
    // );
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
