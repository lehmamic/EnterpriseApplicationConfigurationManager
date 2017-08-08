import { Routes } from '@angular/router';
import { AllProjectsComponent } from 'app/projects/all-projects';
import { AddProjectComponent } from 'app/projects/add-project';

export const projectsRoutes: Routes = [
  {
    path: 'projects',
    component: AllProjectsComponent
  },
  {
    path: 'projects/add',
    component: AddProjectComponent
  }
];
