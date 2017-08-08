import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';

import { SharedModule } from '../shared';
import { AllProjectsComponent } from './all-projects/all-projects.component';
import { ProjectsService } from "./projects.service";
import { AddProjectComponent } from './add-project/add-project.component';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    SharedModule
  ],
  declarations: [ AllProjectsComponent, AddProjectComponent ],
  providers: [ ProjectsService ]
})
export class ProjectsModule { }
