$(document).ready(function()
{
    setInterval(LoadData, 1000);
});

function LoadData()
{
    $.getJSON("", function(data)
    {
        $.each(data, function(key, val)
        {
            if (key == 'headshots')
                $('#' + key).html(val + ' (' + parseFloat((data['kills'] > 0 ? (data['headshots'] / data['kills']) * 100 : data['headshots'])).toFixed(2) + '% of kills)');
            else
                $('#' + key).html(val);
        });
        
        $('#experience_bar').width(data['exp_percent'] + '%');
        $('#online_status').html('Profile - ' + (data['online'] == 1 ? '<span style="color: #19FF19;">Online</span>' : '<span style="color: #D12B2B;">Offline</span>'));
    }); 
}