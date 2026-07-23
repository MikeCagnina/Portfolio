import { Component } from '@angular/core';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  formData = {
    name: '',
    email: '',
    message: ''
  };

  submitted = false;

  onSubmit(): void {
    if (this.formData.name && this.formData.email && this.formData.message) {
      this.submitted = true;
      console.log('Form submitted:', this.formData);
      
      // Reset form after 3 seconds
      setTimeout(() => {
        this.formData = { name: '', email: '', message: '' };
        this.submitted = false;
      }, 3000);
    }
  }
}
