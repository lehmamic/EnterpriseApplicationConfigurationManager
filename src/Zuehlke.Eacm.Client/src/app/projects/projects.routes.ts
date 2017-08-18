import { Routes } from '@angular/router';
import { AllProjectsComponent } from './all-projects';
import { ProjectDetailComponent } from './project-detail';
import { ProjectDetailOverviewComponent } from './project-detail-overview';
import { ProjectDetailSchemaComponent } from './project-detail-schema';
import { AddProjectComponent } from './add-project';

export const projectsRoutes: Routes = [
  {
    path: 'projects',
    component: AllProjectsComponent
  },
  {
    path: 'projects/:id',
    component: ProjectDetailComponent,
    children: [
      { path: '', redirectTo: 'overview', pathMatch: 'full' },
      { path: 'overview', component: ProjectDetailOverviewComponent },
      { path: 'schema', component: ProjectDetailSchemaComponent }
    ]
  },
  {
    path: 'new/project',
    component: AddProjectComponent
  }
];
