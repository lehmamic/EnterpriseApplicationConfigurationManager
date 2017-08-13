
import { Injectable } from '@angular/core';
import { Effect, Actions } from '@ngrx/effects';
import { Action } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import { of } from 'rxjs/observable/of';

import { LOAD_PROJECTS,  CREATE_PROJECT, LoadProjectsSuccessAction, LoadProjectsFailedAction, CreateProjectAction } from './projects.actions';
import { ProjectsService, Project } from './projects.service';
import { Go } from '../shared'

@Injectable()
export class ProjectsEffects {
  @Effect()
  loadProjects$: Observable<Action> = this.actions$
    .ofType(LOAD_PROJECTS)
    .mergeMap(action =>
      this.projects.getProjects()
        // If successful, dispatch success action with result
        .map((data: Array<Project>) => new LoadProjectsSuccessAction(data))
        // If request fails, dispatch failed action
        .catch(() => of(new LoadProjectsFailedAction()))
    );

  @Effect()
  createProject$: Observable<Action> = this.actions$
    .ofType(CREATE_PROJECT)
    .mergeMap((action: CreateProjectAction) =>
      this.projects.createProject(action.payload)
        // If successful, dispatch success action with result
        .map((data: Project) => new Go({ path: ['projects'] }))
        // If request fails, dispatch failed action
        .catch(() => of(new LoadProjectsFailedAction()))
    );

  constructor(private actions$: Actions, private projects: ProjectsService) { }
}
