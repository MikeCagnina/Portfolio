import { Component } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {
  requestId: string | null = null;

  constructor() {
    // Generate a simple request ID for demo purposes
    this.requestId = this.generateRequestId();
  }

  /**
   * Gets a value indicating whether to show the request ID.
   */
  get showRequestId(): boolean {
    return this.requestId !== null && this.requestId.length > 0;
  }

  /**
   * Generates a simple request ID for demo purposes.
   */
  private generateRequestId(): string {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }
}
