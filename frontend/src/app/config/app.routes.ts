import { Routes } from '@angular/router';
import { AlgorithmsComponent } from '../pages/algorithms/algorithms.component';
import { CodeletSettingsComponent } from '../pages/codelet-settings/codelet-settings.component';
import { CreateCodeletComponent } from '../pages/create-codelet/create-codelet.component';
import { HomeComponent } from '../pages/home/home.component';
import { IndexComponent } from '../pages/index/index.component';
import { LoginComponent } from '../pages/login/login.component';
import { PopularCodeletsComponent } from '../pages/popular-codelets/popular-codelets.component';
import { RecentCodeletsComponent } from '../pages/recent-codelets/recent-codelets.component';
import { RegisterComponent } from '../pages/register/register.component';
import { SharedComponent } from '../pages/shared/shared.component';
import { UserSettingsComponent } from '../pages/user-settings/user-settings.component';

export const routes: Routes = [
  { path: '', redirectTo: 'index', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'index', component: IndexComponent },
  { path: 'algorithms', component: AlgorithmsComponent },
  { path: 'codelet-settings', component: CodeletSettingsComponent },
  { path: 'create-codelet', component: CreateCodeletComponent },
  { path: 'login', component: LoginComponent },
  { path: 'popular-codelets', component: PopularCodeletsComponent },
  { path: 'recent-codelets', component: RecentCodeletsComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'shared', component: SharedComponent },
  { path: 'user-settings', component: UserSettingsComponent }
];
