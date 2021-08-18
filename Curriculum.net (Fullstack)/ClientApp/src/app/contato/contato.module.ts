import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContatoRoutingModule } from './contato-routing.module';
import { ContatoComponent } from './contato/contato.component';
import { HomeModule } from '../home/home.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ContatoComponent,
  ],
  imports: [
    CommonModule,
    ContatoRoutingModule,
    HomeModule,
    ReactiveFormsModule
  ]
})
export class ContatoModule { }
