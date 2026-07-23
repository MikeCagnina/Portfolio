import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent {
  technologies = [
    'Angular 17',
    'TypeScript',
    'RxJS',
    'HTML5 & CSS3',
    'Responsive Design'
  ];
}
