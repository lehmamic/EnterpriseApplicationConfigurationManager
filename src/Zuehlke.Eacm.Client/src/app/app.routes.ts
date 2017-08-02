import { Routes } from '@angular/router';

import { projectsRoutes } from './projects/projects.routes'

export const routes: Routes = [
  { path: '', redirectTo: 'projects', pathMatch: 'full' },
  ...projectsRoutes,
];
