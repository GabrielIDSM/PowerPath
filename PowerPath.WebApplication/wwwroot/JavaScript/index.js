$(function () {
    ConfigurarSelect2()
    ConfigurarFiltros()
})

function ConfigurarSelect2() {
    $('#operadora').select2({
        placeholder: 'Operadora',
        minimumResultsForSearch: Infinity,
        allowClear: true
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