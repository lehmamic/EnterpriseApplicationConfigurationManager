import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';

import { NavbarComponent } from './navbar';
import { PageHeaderComponent } from './page-header';
import { FormPanelComponent, FormPanelHeaderComponent, FormPanelBodyComponent, FormPanelActionsComponent } from './form-panel';
import { TileComponent } from './tile/tile.component'

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MaterialModule
  ],
  declarations: [
    NavbarComponent,
    PageHeaderComponent,
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent,
    TileComponent
  ],
  exports: [
    NavbarComponent,
    PageHeaderComponent,
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent,
    TileComponent
  ],
  providers: [ ]
})
export class SharedModule { }
