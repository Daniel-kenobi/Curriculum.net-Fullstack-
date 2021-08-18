import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CadastrarRoutingModule } from './cadastrar-routing.module';
import { CadastrarComponent } from './cadastrar/cadastrar.component';
import { HomeModule } from '../home/home.module';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [CadastrarComponent],
  imports: [
    CommonModule,
    CadastrarRoutingModule,
    HomeModule,
    ReactiveFormsModule
  ]
})
export class CadastrarModule { }
