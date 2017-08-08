import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-panel-header',
  template: '<ng-content></ng-content>',
  //changeDetection: ChangeDetectionStrategy.OnPush
})
export class FormPanelHeaderComponent { }
