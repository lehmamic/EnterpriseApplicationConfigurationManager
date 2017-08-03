import { Action } from '@ngrx/store';
import { Project } from "./projects.service";

export const LOAD_PROJECTS_SUCCESS = '[Projects] LOAD_SUCCESS';
export const LOAD_PROJECTS_FAILED = '[Projects] LOAD_FAILED';
export const LOAD_PROJECTS = '[Projects] LOAD';

export class LoadProjectsSuccessAction implements Action {
  readonly type = LOAD_PROJECTS_SUCCESS;

  constructor(public payload: Array<Project>) {}
}

export class LoadProjectsFailedAction implements Action {
  readonly type = LOAD_PROJECTS_FAILED;
}

export class LoadProjectsAction implements Action {
  readonly type = LOAD_PROJECTS;
}

export type Actions =
   LoadProjectsSuccessAction
 | LoadProjectsFailedAction
 | LoadProjectsAction;
