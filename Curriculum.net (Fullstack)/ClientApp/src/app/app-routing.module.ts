import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { GuardServiceGuard } from './services/guard-service.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: 'home', loadChildren: () => import('./home/home.module').then(x => x.HomeModule) },
  { path: 'contato', loadChildren: () => import('./contato/contato.module').then(x => x.ContatoModule), canLoad: [GuardServiceGuard] },
  { path: 'login', loadChildren: () => import('./login/login.module').then(x => x.LoginModule) },
  { path: 'cadastrar', loadChildren: () => import('./cadastrar/cadastrar.module').then(x => x.CadastrarModule) },
  { path: 'perfil', loadChildren: () => import('./perfil/perfil.module').then(x => x.PerfilModule), canLoad: [GuardServiceGuard] }
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
