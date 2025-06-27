import { Routes } from '@angular/router';
import { CodeletSettingsComponent } from '../pages/codelet-settings/codelet-settings.component';
import { CreateCodeletComponent } from '../pages/create-codelet/create-codelet.component';
import { HomeComponent } from '../pages/home/home.component';
import { LoginComponent } from '../pages/login/login.component';
import { PopularCodeletsComponent } from '../pages/popular-codelets/popular-codelets.component';
import { RecentCodeletsComponent } from '../pages/recent-codelets/recent-codelets.component';
import { RegisterComponent } from '../pages/register/register.component';
import { MyProfileComponent } from '../pages/my-profile/my-profile.component';
import { PublicProfileComponent } from '../pages/public-profile/public-profile.component';
import { CodeletComponent } from '../pages/codelet/codelet.component';
import { SearchResultsComponent } from '../pages/search-results/search-results.component';

import { GuestGuard } from '../guards/guest.guard';
import { AuthGuard } from '../guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'codelet-settings', component: CodeletSettingsComponent, canActivate: [AuthGuard] },
  { path: 'create-codelet', component: CreateCodeletComponent, canActivate: [AuthGuard]},
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [GuestGuard] },
  { path: 'popular-codelets', component: PopularCodeletsComponent },
  { path: 'recent-codelets', component: RecentCodeletsComponent },
  { path: 'my-profile', component: MyProfileComponent, canActivate: [AuthGuard] },
  { path: 'user/:username', component: PublicProfileComponent },
  { path: 'codelet/:id', component: CodeletComponent },
  { path: 'search-results',  component: SearchResultsComponent}
];
