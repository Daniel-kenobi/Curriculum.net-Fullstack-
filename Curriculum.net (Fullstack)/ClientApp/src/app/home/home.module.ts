import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MenuModule } from '../menu/menu.module';
import { ErrorModule } from '../error/error.module';
import { DialogModule } from '../dialog/dialog.module';
import { DialogService } from '../dialog/dialogservice.service';
import { MatSelectModule, MatCheckboxModule, MatExpansionModule } from '@angular/material';

@NgModule({
  declarations: [
    HomeComponent,
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    ReactiveFormsModule,
    MenuModule,
    ErrorModule,
    DialogModule,
    MatSelectModule,
    MatCheckboxModule,
    MatExpansionModule
  ],
  providers: [
    DialogService
  ]
})
export class HomeModule { }
