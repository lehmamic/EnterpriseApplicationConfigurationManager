import { Routes } from '@angular/router';
import { AllProjectsComponent } from './all-projects';
import { ProjectDetailComponent } from './project-detail';
import { AddProjectComponent } from './add-project';

export const projectsRoutes: Routes = [
  {
    path: 'projects',
    component: AllProjectsComponent
  },
  {
    path: 'projects/:id',
    component: ProjectDetailComponent
  },
  {
    path: 'new/project',
    component: AddProjectComponent
  }
];
