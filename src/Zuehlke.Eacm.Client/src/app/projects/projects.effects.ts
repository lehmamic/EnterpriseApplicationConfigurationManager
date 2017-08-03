
import { Injectable } from "@angular/core";
import { Effect, Actions } from "@ngrx/effects";
import { Action } from "@ngrx/store";
import { Observable } from "rxjs/Observable";

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import { of } from 'rxjs/observable/of';

import { LOAD_PROJECTS, LoadProjectsSuccessAction, LoadProjectsFailedAction } from "app/projects/project.actions";
import { ProjectsService, Project } from "app/projects/projects.service";

@Injectable()
export class ProjectsEffects {
  @Effect()
  search$: Observable<Action> = this.actions$
    .ofType(LOAD_PROJECTS)
    .mergeMap(action =>
      this.projects.getProjects()
        // If successful, dispatch success action with result
        .map((data: Array<Project>) => new LoadProjectsSuccessAction(data))
        // If request fails, dispatch failed action
        .catch(() => of(new LoadProjectsFailedAction()))
    );

  constructor(private actions$: Actions, private projects: ProjectsService) { }
}
