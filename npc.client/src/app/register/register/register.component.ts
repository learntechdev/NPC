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



  url: any; 
  msg = "";
  selectedImage: any;


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


  selectFile(event: any) { //Angular 11, for stricter type
    if (!event.target.files[0] || event.target.files[0].length == 0) {
      this.msg = 'You must select an image';
      return;
    }

    var mimeType = event.target.files[0].type;

    if (mimeType.match(/image\/*/) == null) {
      this.msg = "Only images are supported";
      return;
    }

    var reader = new FileReader();
    this.selectedImage = event.target.files[0];
    reader.readAsDataURL(event.target.files[0]);

    reader.onload = (_event) => {
      this.msg = "";
      this.url = reader.result;
    }
  }
  RemoveImage() {
    this.msg = "";
    this.url = "";
    this.selectedImage = "";
  }
  Register() {
    console.log(this.selectedImage);
  }

}
