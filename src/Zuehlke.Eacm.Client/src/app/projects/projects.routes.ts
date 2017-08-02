import { Routes } from '@angular/router';
import { AllProjectsComponent } from "app/projects/all-projects/all-projects.component";

export const projectsRoutes: Routes = [
  {
    path: 'projects',
    component: AllProjectsComponent
  }
];
