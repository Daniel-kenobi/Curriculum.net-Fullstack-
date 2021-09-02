import { Component, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';
import { cadastroModel } from '../../models/cadastro.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html'
})
export class PerfilComponent implements OnInit {
  usuario_logado: usuarioModel;
  perfilGroup: FormGroup;
  imagem: string;
  primeironome: string;

  constructor(private authService: authService, private frmBuilder: FormBuilder, private router: Router) {
  }

  ngOnInit() {
    this.usuario_logado = this.authService.usrLogado;

    if (this.usuario_logado.img_perfil) {
      this.usuario_logado.img_perfil = ("data:image/png;base64," + this.usuario_logado.img_perfil);

      if (this.usuario_logado.img_perfil.length > 0)
        this.imagem = this.usuario_logado.img_perfil;
    }

    if (this.usuario_logado) {
      this.perfilGroup = this.frmBuilder.group({
        img_perfil: [''],
        Nome: [this.usuario_logado.nome || '', [Validators.required]],
        Email: [this.usuario_logado.email || '', [Validators.required]],
        Telefone: [this.usuario_logado.telefone || '', [Validators.required]],
        instagram: [this.usuario_logado.instagram || ''],
        linkedin: [this.usuario_logado.linkedin || ''],
        github: [this.usuario_logado.github || '']
      })
    }
  }

  onFileChange(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onloadend = () => {
        const image_bytes = reader.result.toString();
        this.imagem = image_bytes;
        this.perfilGroup.patchValue({
          img_perfil: image_bytes 
        });
      };
    }
  }

  fnc_monta_modelo(): cadastroModel {

    const modelo = new cadastroModel();
    modelo.nome = this.perfilGroup.controls['Nome'].value;
    modelo.email = this.perfilGroup.controls['Email'].value;
    modelo.telefone = this.perfilGroup.controls['Telefone'].value;
    modelo.instagram = this.perfilGroup.controls['instagram'].value;
    modelo.linkedin = this.perfilGroup.controls['linkedin'].value;
    modelo.github = this.perfilGroup.controls['github'].value;
    modelo.img_perfil = this.perfilGroup.controls['img_perfil'].value.replace("data:image/png;base64,", "");
    return modelo;

  }

  onSubmit() {
    this.authService.fnc_atualiza_credenciais(this.fnc_monta_modelo()).subscribe(x =>
    {
      this.router.navigateByUrl('home');
      this.authService.fnc_altera_usuario_logado(this.authService.fnc_transforma_modelos(x));
    }, (err) => { console.log(err) })
  }
}
