import { Component, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html'
})
export class PerfilComponent implements OnInit, OnChanges {
  usuario_logado: usuarioModel;
  perfilGroup: FormGroup;
  imagem: string;
  primeironome: string;

  constructor(private authService: authService, private frmBuilder: FormBuilder, private changes: ChangeDetectorRef) {
    this.authService.fnc_redireciona();
  }

  ngOnInit() {
    this.usuario_logado = this.authService.usrLogado;
    this.primeironome = this.usuario_logado.nome.substr(0, this.usuario_logado.nome.indexOf(' '));

    if (this.usuario_logado.img_perfil) {
      this.usuario_logado.img_perfil = ("data:image/png;base64," + this.usuario_logado.img_perfil);

      if (this.usuario_logado.img_perfil.length > 0)
        this.imagem = this.usuario_logado.img_perfil;
    }

    if (this.usuario_logado) {
      this.perfilGroup = this.frmBuilder.group({
        img_perfil: [''],
        Nome: [this.usuario_logado.nome || ''],
        Email: [this.usuario_logado.email || ''],
        Telefone: [this.usuario_logado.telefone || ''],
        instagram: [this.usuario_logado.instagram || ''],
        linkedin: [this.usuario_logado.linkedin || ''],
        github: [this.usuario_logado.github || '']
      })
    }
  }

  ngOnChanges() {
    console.log("ok");
  }

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onloadend = (e) => {
        const image_bytes = reader.result.toString();

        this.changes.detectChanges();
        this.imagem = image_bytes;
        this.changes.markForCheck();

        this.perfilGroup.patchValue({
          img_perfil: image_bytes
        });
      };
    }
  }

  onSubmit() {
    this.authService.fnc_atualiza_credenciais().subscribe(x => { }, (err) => { console.log(err) })
  }

}
