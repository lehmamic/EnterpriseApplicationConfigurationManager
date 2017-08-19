import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';

import { NavbarComponent } from './navbar';
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
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent,
    TileComponent
  ],
  exports: [
    NavbarComponent,
    FormPanelComponent,
    FormPanelHeaderComponent,
    FormPanelBodyComponent,
    FormPanelActionsComponent,
    TileComponent
  ],
  providers: [ ]
})
export class SharedModule { }
