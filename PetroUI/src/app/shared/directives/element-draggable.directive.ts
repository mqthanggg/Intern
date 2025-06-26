import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[ElementDraggable]',
  standalone: true
})
export class ElementDraggableDirective {
  private draggableElement: HTMLDivElement;

  constructor(private elRef: ElementRef<HTMLDivElement>) {
    this.draggableElement = this.elRef.nativeElement;
    this.draggableElement.style.position = 'absolute';
  }

  getElement(): HTMLDivElement {
    return this.draggableElement;
  }
}

@Directive({
  selector: '[ElementDraggableSection]',
  standalone: true
})
export class ElementDraggableSectionDirective {
  private _startX = 0;
  private _startY = 0;
  private _newX = 0;
  private _newY = 0;

  constructor(private elRef: ElementRef<HTMLDivElement>, private draggableDirective: ElementDraggableDirective) {
    const dragSectionElement = this.elRef.nativeElement;
    const draggableElement = this.draggableDirective.getElement();

    dragSectionElement.addEventListener('mouseover', () => {
      dragSectionElement.style.cursor = 'pointer';
    });

    dragSectionElement.addEventListener('mousedown', (event) => {
      dragSectionElement.style.cursor = 'move';
      this._startX = event.clientX;
      this._startY = event.clientY;

      document.addEventListener('mousemove', mouseMove);
      document.addEventListener('mouseup', mouseUp);
    });

    const mouseMove = (e: MouseEvent) => {
      this._newX = this._startX - e.clientX;
      this._newY = this._startY - e.clientY;

      this._startX = e.clientX;
      this._startY = e.clientY;

      if (draggableElement) {
        draggableElement.style.top = (draggableElement.offsetTop - this._newY) + 'px';
        draggableElement.style.left = (draggableElement.offsetLeft - this._newX) + 'px';
      }
    };

    const mouseUp = () => {
      document.removeEventListener('mousemove', mouseMove);
    };
  }
}