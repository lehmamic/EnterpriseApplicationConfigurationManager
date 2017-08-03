import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllProjectsComponent } from './all-projects/all-projects.component';
import { ProjectsService } from "./projects.service";

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [ AllProjectsComponent ],
  providers: [ ProjectsService ]
})
export class ProjectsModule { }
