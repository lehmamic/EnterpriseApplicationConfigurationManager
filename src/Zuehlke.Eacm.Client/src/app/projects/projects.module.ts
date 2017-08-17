import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared';
import { AllProjectsComponent } from './all-projects/all-projects.component';
import { ProjectsService } from "./projects.service";
import { AddProjectComponent } from './add-project/add-project.component';
import { ProjectTileComponent } from './project-tile/project-tile.component';


@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MaterialModule,
    SharedModule
  ],
  declarations: [ AllProjectsComponent, AddProjectComponent, ProjectTileComponent ],
  providers: [ ProjectsService ]
})
export class ProjectsModule { }
