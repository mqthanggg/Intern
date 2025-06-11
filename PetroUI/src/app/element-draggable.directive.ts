import { Directive, ElementRef } from '@angular/core';

let draggableElement: HTMLDivElement;

@Directive({
  selector: '[ElementDraggable]',
  standalone: true
})
export class ElementDraggableDirective {
  constructor(private elRef: ElementRef<HTMLDivElement>) {
    draggableElement = this.elRef.nativeElement
    draggableElement.style.position = 'absolute'
  }
}

@Directive({
  selector: '[ElementDraggableSection]',
  standalone: true
})
export class ElementDraggableSectionDirective {

  constructor(private elRef: ElementRef<HTMLDivElement>) {
    let _startX = 0;
    let _startY = 0;
    let _newX = 0;
    let _newY = 0;
    let draggleSectionElement = this.elRef.nativeElement;
    draggleSectionElement.addEventListener('mouseover', (event) => {
      draggleSectionElement.style.cursor = "pointer"
    })
    draggleSectionElement.addEventListener('mousedown', mouseDown)

    function mouseDown(e: MouseEvent){
        draggleSectionElement.style.cursor = 'move'
        _startX = e.clientX
        _startY = e.clientY

        document.addEventListener('mousemove', mouseMove)
        document.addEventListener('mouseup', mouseUp)
    }

    function mouseMove(e: MouseEvent){
        _newX = _startX - e.clientX 
        _newY = _startY - e.clientY 
      
        _startX = e.clientX
        _startY = e.clientY

        draggableElement.style.top = (draggableElement.offsetTop - _newY) + 'px'
        draggableElement.style.left = (draggableElement.offsetLeft - _newX) + 'px'
    }

    function mouseUp(e: MouseEvent){
        document.removeEventListener('mousemove', mouseMove)
    }
  }

}
