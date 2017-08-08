import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';

import { NavbarComponent } from './navbar';
import { PageHeaderComponent } from './page-header';
import { FormActionsComponent } from './form-actions/form-actions.component';
import { FormPanelComponent, FormPanelHeaderComponent, FormPanelBodyComponent, FormPanelActionsComponent } from './form-panel'

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule
  ],
  declarations: [
    NavbarComponent,
    PageHeaderComponent,
    FormActionsComponent,
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent
  ],
  exports: [
    NavbarComponent,
    PageHeaderComponent,
    FormActionsComponent,
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent
  ],
  providers: [ ]
})
export class SharedModule { }
