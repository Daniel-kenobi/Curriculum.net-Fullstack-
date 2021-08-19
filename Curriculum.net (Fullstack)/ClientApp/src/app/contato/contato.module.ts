import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContatoRoutingModule } from './contato-routing.module';
import { ContatoComponent } from './contato/contato.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ErrorModule } from '../error/error.module';
import { MenuModule } from '../menu/menu.module';

@NgModule({
  declarations: [
    ContatoComponent,
  ],
  imports: [
    CommonModule,
    ContatoRoutingModule,
    ReactiveFormsModule,
    ErrorModule,
    MenuModule
  ]
})
export class ContatoModule { }
