import { Project } from './projects.service';

export const initialProjectState: ProjectsState = {
  isProcessing: false,
  projects: []
};

export interface ProjectsState {
  isProcessing: boolean;
  projects: Array<Project>
};
