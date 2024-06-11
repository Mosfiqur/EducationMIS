import { Directive, ElementRef} from '@angular/core';
@Directive({
   selector: '[listTypeCellRenderer]'
})
export class ListTypeCellRendererDirective {
   constructor(Element: ElementRef) {
      console.log(Element);
      Element.nativeElement.innerText = "Text is changed by changeText Directive.";
   }
}