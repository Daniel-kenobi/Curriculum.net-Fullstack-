import { Component, AfterViewChecked, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DialogService } from '../../dialog/dialogservice.service';
import { CurriculumModel } from '../../models/curriculo.model';
import { authService } from '../../services/auth.service';
import { cepService } from '../../services/cep.service';
import { curriculoService } from '../../services/curriculo.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements AfterViewChecked {
  curriculoGroup: FormGroup;
  curriculo: CurriculumModel = new CurriculumModel();
  private FrasePadrao: string = "Me encantaria encontrar uma vaga para essa empresa que é uma instituição que admiro tanto. Além disso, acredito que minha desenvoltura " +
    "natural com pessoas, ótima comunicação, e jeito cuidadoso se provarão muito úteis. Gostaria de poder falar sobre como posso contribuir para" +
    " essa empresa, e contando com experiência para o meu aperfeiçoamento pessoal, já agradeço pela futura resposta positiva!";

  private panelOpenState = false;

  constructor(private frmBuilder: FormBuilder, private cepService: cepService, private curriculoService: curriculoService, private authService: authService, private dialogService: DialogService) {
    this.curriculoGroup = this.frmBuilder.group(
      {
        ID: 0,
        Nome: ['', [Validators.required, Validators.minLength(3)]],
        Email: ['', [Validators.email, Validators.required]],
        Telefone: ['', [Validators.minLength(10), Validators.maxLength(11), Validators.required]],
        Endereco: this.frmBuilder.group({
          cep: ['', [Validators.required, Validators.minLength(8)]],
          Logradouro: [''],
          Complemento: [''],
          Bairro: [''],
          Localidade: [''],
          UF: ['']
        }),
        FraseMotivacional: [''],
        Linkedin: [''],
        GitHub: [''],
        Instagram: [''],
        lst_infos_academicas: this.frmBuilder.array([]),
        lst_historico_profissional: this.frmBuilder.array([]),
        lst_soft_skills: this.frmBuilder.array([]),
        lst_qualidades: this.frmBuilder.array([]),
        lst_idiomas: this.frmBuilder.array([]),
        template: ['']
      });
  }

  fnc_altera_estado_expansion() {
    this.panelOpenState = !this.panelOpenState;
  }

  ngAfterViewChecked() {
    if (this.authService.fnc_retorna_usuario_logado()) {
      this.curriculoGroup.controls['Nome'].setValue(this.authService.fnc_retorna_usuario_logado().nome);
      this.curriculoGroup.controls['Email'].setValue(this.authService.fnc_retorna_usuario_logado().email);
      this.curriculoGroup.controls['Telefone'].setValue(this.authService.fnc_retorna_usuario_logado().telefone);
      this.curriculoGroup.controls['Instagram'].setValue(this.authService.fnc_retorna_usuario_logado().instagram);
      this.curriculoGroup.controls['GitHub'].setValue(this.authService.fnc_retorna_usuario_logado().github);
      this.curriculoGroup.controls['Linkedin'].setValue(this.authService.fnc_retorna_usuario_logado().linkedin);
    }
  }

  fnc_adiciona_infos_academicas() {
    const infos = this.curriculoGroup.controls.lst_infos_academicas as FormArray;

    infos.push(this.frmBuilder.group(
      {
        nome_instituicao: [''],
        tipocurso: [''],
        curso: [''],
        descricao_aprendizado: [''],
        datainicio: [''],
        dataconclusao: ['']
      }))
  }

  fnc_remove_infos_academicas() {
    const infos = this.curriculoGroup.controls.lst_infos_academicas as FormArray;

    if (infos.length <= 0)
      return;

    infos.removeAt(infos.length - 1);
  }

  fnc_adiciona_Historico_Profissional() {
    const infos = this.curriculoGroup.controls.lst_historico_profissional as FormArray;

    infos.push(this.frmBuilder.group(
      {
        Nome_instituicao: ['', [Validators.required]],
        Cargo: ['', [Validators.required]],
        Descricao_cargo: ['', [Validators.required]],
        DataInicio: ['', [Validators.required]],
        DataSaida: [''],
        atual: ['']
      }))
  }

  fnc_remove_Historico_Profissional() {
    const infos = this.curriculoGroup.controls.lst_historico_profissional as FormArray;

    if (infos.length <= 0)
      return;

    infos.removeAt(infos.length - 1);
  }

  fnc_adiciona_lst_soft_skills() {
    const infos = this.curriculoGroup.controls.lst_soft_skills as FormArray;

    infos.push(this.frmBuilder.group(
      {
        Nome: ['', [Validators.required]],
        Descricao: ['', [Validators.required]]
      }))
  }

  fnc_remove_lst_soft_skills() {
    const infos = this.curriculoGroup.controls.lst_soft_skills as FormArray;

    if (infos.length <= 0)
      return;

    infos.removeAt(infos.length - 1);
  }

  fnc_adiciona_lst_idiomas() {
    const infos = this.curriculoGroup.controls.lst_idiomas as FormArray;

    infos.push(this.frmBuilder.group(
      {
        Idioma: ['', [Validators.required]],
        Nivel: ['', [Validators.required]]
      }))
  }

  fnc_remove_lst_idiomas() {
    const infos = this.curriculoGroup.controls.lst_idiomas as FormArray;

    if (infos.length <= 0)
      return;

    infos.removeAt(infos.length - 1)
  }


  fnc_adiciona_lst_qualidades() {
    const infos = this.curriculoGroup.controls.lst_qualidades as FormArray;

    infos.push(this.frmBuilder.group(
      {
        Nome: ['', [Validators.required]]
      }))
  }

  fnc_remove_lst_qualidades() {
    const infos = this.curriculoGroup.controls.lst_qualidades as FormArray;

    if (infos.length <= 0)
      return;

    infos.removeAt(infos.length - 1)
  }


  fnc_cria_modelo(): CurriculumModel {
    const curriculo = new CurriculumModel();

    curriculo.ID = this.curriculoGroup.controls['ID'].value;
    curriculo.Nome = this.curriculoGroup.controls['Nome'].value;
    curriculo.Email = this.curriculoGroup.controls['Email'].value;
    curriculo.Telefone = this.curriculoGroup.controls['Telefone'].value;
    curriculo.Endereco = this.curriculoGroup.controls['Endereco'].value;
    curriculo.FraseMotivacional = this.curriculoGroup.controls['FraseMotivacional'].value;
    curriculo.Linkedin = this.curriculoGroup.controls['Linkedin'].value;
    curriculo.GitHub = this.curriculoGroup.controls['GitHub'].value;
    curriculo.Instagram = this.curriculoGroup.controls['Instagram'].value;
    curriculo.lst_infos_academicas = this.curriculoGroup.controls['lst_infos_academicas'].value;
    curriculo.lst_Historico_Profissional = this.curriculoGroup.controls['lst_historico_profissional'].value;
    curriculo.lst_soft_skills = this.curriculoGroup.controls['lst_soft_skills'].value;
    curriculo.lst_idiomas = this.curriculoGroup.controls['lst_idiomas'].value;
    curriculo.lst_qualidades = this.curriculoGroup.controls['lst_qualidades'].value;
    curriculo.template = this.curriculoGroup.controls['template'].value;

    return curriculo;
  }

  onSubmit() {
    const curriculo = this.fnc_cria_modelo();

    console.log(curriculo);

    this.curriculoService.fnc_cria_curriculo(curriculo).subscribe((x) => {

      var file = new Blob([x], { type: 'application/pdf' });
      var fileURL = URL.createObjectURL(file);
      window.open(fileURL, '_blank');

    }, (err) => { console.log(err) });
  }
}
