import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from "rxjs/Observable";

import 'rxjs/add/operator/map';

export interface Project {
  name: string;
  description: string;
}

@Injectable()
export class ProjectsService {
  constructor(private http: Http) {
  }

  getProjects(): Observable<Array<Project>> {
    return this.http.get('api/projects')
      .map(res => res.json() || [])
  }

  createProject(project: Project): Observable<Project> {
    return this.http.post('api/projects', project)
      .map(res => res.json());
  }
}
