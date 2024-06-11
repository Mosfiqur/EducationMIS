import { Directive, OnInit, Inject, ElementRef, Input } from "@angular/core";
import { JQ_TOKEN } from "../../../services/jQuery.service";

@Directive({
  selector: '[instance-modal-trigger]'
})
export class InstanceModalTriggerDirective implements OnInit {
  private el: HTMLElement;
  @Input('instance-modal-trigger') modalId: string;

  constructor(ref: ElementRef, @Inject(JQ_TOKEN) private $ : any) {
    this.el = ref.nativeElement;
  } 
  
  ngOnInit() {
    this.el.addEventListener('click', e => {
      this.$(`#${this.modalId}`).modal({})
    })
  }
  
}