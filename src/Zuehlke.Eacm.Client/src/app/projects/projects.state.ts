import { Project } from './projects.service';

export const initialProjectState: ProjectsState = {
  isLoadingProjects: false,
  projects: []
};

export interface ProjectsState {
  isLoadingProjects: boolean;
  projects: Array<Project>
};
