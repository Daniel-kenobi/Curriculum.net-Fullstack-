import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CadastrarRoutingModule } from './cadastrar-routing.module';
import { CadastrarComponent } from './cadastrar/cadastrar.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MenuModule } from '../menu/menu.module';
import { ErrorModule } from '../error/error.module';


@NgModule({
  declarations: [CadastrarComponent],
  imports: [
    CommonModule,
    CadastrarRoutingModule,
    ReactiveFormsModule,
    MenuModule,
    ErrorModule
  ]
})
export class CadastrarModule { }
