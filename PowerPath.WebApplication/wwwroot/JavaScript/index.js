$(function () {
    ConfigurarSelect2()
    ConfigurarFiltros()
    ExibirMensagem()
})

function ConfigurarSelect2() {
    $('#operadora').select2({
        placeholder: 'Operadora',
        minimumResultsForSearch: Infinity,
        allowClear: true
    })

    $('#Operadora').select2({
        placeholder: 'Operadora',
        minimumResultsForSearch: Infinity,
        allowClear: false
    })
}

function ConfigurarFiltros() {
    $('#instalacao').on('input', FiltrarMedidores)
    $('#lote').on('input', FiltrarMedidores)
    $('#operadora').on('change.select2', FiltrarMedidores)
    $('#fabricante').on('input', FiltrarMedidores)
    $('#modelo').on('input', FiltrarMedidores)
    $('#versao').on('input', FiltrarMedidores)
}

function ExibirMensagem() {
    let mensagemSucesso = sessionStorage.getItem('MensagemSucesso'),
        mensagemErro = sessionStorage.getItem('MensagemErro')

    if (mensagemSucesso) {
        $(`
            <div class="Notificacao Sucesso">
		        <p>${mensagemSucesso}</p>
		        <button class="btn" onclick="OnClickFecharNotificacao()"><i class="fa-solid fa-xmark"></i></button>
	        </div>
        `).appendTo($('body'))
        sessionStorage.removeItem('MensagemSucesso')
    } else if (mensagemErro) {
        $(`
            <div class="Notificacao Erro">
		        <p>${mensagemErro}</p>
		        <button class="btn" onclick="OnClickFecharNotificacao()"><i class="fa-solid fa-xmark"></i></button>
	        </div>
        `).appendTo($('body'))
        sessionStorage.removeItem('MensagemErro')
    }
}

function AdicionarMensagemSucesso(mensagem) {
    sessionStorage.setItem('MensagemSucesso', mensagem)
}

function AdicionarMensagemErro(mensagem) {
    sessionStorage.setItem('MensagemErro', mensagem)
}

function FiltrarMedidores() {
    let $medidores = $('#Medidores .Item'),
        instalacao = $('.Filtros #instalacao').val(),
        lote = parseInt($('.Filtros #lote').val()),
        operadora = $('.Filtros #operadora').val(),
        fabricante = $('.Filtros #fabricante').val(),
        modelo = parseInt($('.Filtros #modelo').val()),
        versao = parseInt($('.Filtros #versao').val())

    $medidores.css('display', 'unset')
    $medidores.each(function () {
        let $medidor = $(this),
            exibir = true

        if (instalacao && instalacao !== '')
            exibir = exibir && $medidor.data('instalacao').includes(instalacao)

        if (lote)
            exibir = exibir && parseInt($medidor.data('lote')) == lote

        if (operadora && operadora !== '')
            exibir = exibir && $medidor.data('operadora') == operadora

        if (fabricante && fabricante !== '')
            exibir = exibir && $medidor.data('fabricante').includes(fabricante)

        if (modelo) 
            exibir = exibir && parseInt($medidor.data('modelo')) == modelo

        if (versao)
            exibir = exibir && parseInt($medidor.data('versao')) == versao

        if (!exibir)
            $medidor.css('display', 'none')
    })
}

function OnClickFecharNotificacao() {
    $('.Notificacao').remove()
}

function OnClickExcluirMedidor(instalacao, lote) {
    if (window.confirm(`Deseja excluir o medidor de Instalação: ${instalacao} e Lote: ${lote}?`)) {
        $.ajax({
            url: 'Medidor/Excluir/',
            type: 'DELETE',
            data: { instalacao: instalacao, lote: lote },
            success: function (response) {
                AdicionarMensagemSucesso(response.mensagem)
                location.reload()
            },
            error: function (xhr) {
                AdicionarMensagemErro(xhr.responseJSON.mensagem)
            }
        });
    }
}