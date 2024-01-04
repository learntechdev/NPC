import { Component } from '@angular/core';


interface Gender {
  Id: number;
  Name: string;
}
interface Position {
  Id: number;
  Name: string;
}

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  selectedIdentityType!: string;
  genders: Gender[] | undefined;
  selectedGender: Gender | undefined;
  positions: Position[] | undefined;
  selectedPosition: Position | undefined;

  customcss: { [klass: string]: any } = { "width": "100%","line-height":"1rem"};

  ngOnInit() {
    this.genders = [
      { Id: 1, Name: 'Male' },
      { Id: 2, Name: 'Female' }      
    ];
    this.positions = [
      { Id: 1, Name: 'Senior' },
      { Id: 2, Name: 'Junior' }
    ];
  }

}
