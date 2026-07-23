import { Component } from '@angular/core';

@Component({
  selector: 'app-counter',
  templateUrl: './counter.component.html',
  styleUrls: ['./counter.component.css']
})
export class CounterComponent {
  currentCount = 0;

  /**
   * Increments the current count.
   */
  incrementCount(): void {
    this.currentCount++;
  }
}
