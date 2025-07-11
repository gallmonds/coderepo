import { Routes } from '@angular/router';
import { CodeletSettingsComponent } from '../pages/codelet-settings/codelet-settings.component';
import { CreateCodeletComponent } from '../pages/create-codelet/create-codelet.component';
import { HomeComponent } from '../pages/home/home.component';
import { LoginComponent } from '../pages/login/login.component';
import { PopularCodeletsComponent } from '../pages/popular-codelets/popular-codelets.component';
import { RecentCodeletsComponent } from '../pages/recent-codelets/recent-codelets.component';
import { RegisterComponent } from '../pages/register/register.component';
import { PublicProfileComponent } from '../pages/public-profile/public-profile.component';
import { CodeletComponent } from '../pages/codelet/codelet.component';
import { SearchResultsComponent } from '../pages/search-results/search-results.component';
import { PageNotFoundComponent } from '../components/page-not-found/page-not-found.component';
import { GuestGuard } from '../guards/guest.guard';
import { AuthGuard } from '../guards/auth.guard';
import { MainLayoutComponent } from '../components/layouts/main-layout.component';

export const routes: Routes = [
{
  path: '',
  component: MainLayoutComponent,
  children: [
    { path: '', component: HomeComponent },
    { path: 'codelet-settings', component: CodeletSettingsComponent, canActivate: [AuthGuard] },
    { path: 'create-codelet', component: CreateCodeletComponent, canActivate: [AuthGuard]},
    { path: 'popular-codelets', component: PopularCodeletsComponent },
    { path: 'recent-codelets', component: RecentCodeletsComponent },
    { path: 'user/:id', component: PublicProfileComponent },
    { path: 'codelet/:id', component: CodeletComponent },
    { path: 'search-results',  component: SearchResultsComponent}
  ] 
},
{ path: 'login', component: LoginComponent, canActivate: [GuestGuard] },
{ path: 'register', component: RegisterComponent, canActivate: [GuestGuard] },
{ path: '404', component: PageNotFoundComponent },
{ path: '**', redirectTo: '/404' }
];
