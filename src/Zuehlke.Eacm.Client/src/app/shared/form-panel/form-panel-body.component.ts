import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-panel-body',
  template: '<ng-content></ng-content>',
  //changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormPanelBodyComponent { }
