import { Routes } from '@angular/router';
import { HomeComponent } from '../pages/home/home.component'
import { IndexComponent } from '../pages/index/index.component'

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'index', component: IndexComponent },

];
