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

import { GuestGuard } from '../guards/guest.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'codelet-settings', component: CodeletSettingsComponent },
  { path: 'create-codelet', component: CreateCodeletComponent },
  { path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
  { path: 'register', component: RegisterComponent, canActivate: [GuestGuard] },
  { path: 'popular-codelets', component: PopularCodeletsComponent },
  { path: 'recent-codelets', component: RecentCodeletsComponent },
  { path: 'my-profile', component: MyProfileComponent },
  { path: 'user/:username', component: PublicProfileComponent },
  { path: 'codelet', component: CodeletComponent }
];
