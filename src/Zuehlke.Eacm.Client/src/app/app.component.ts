import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Store } from '@ngrx/store';

import { RootState } from './app.state';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app works!';

  constructor(private store: Store<RootState>, private titleService: Title) {
    store.select(s => s.app.pageTitle)
         .subscribe((title) => titleService.setTitle(title));
  }
}
