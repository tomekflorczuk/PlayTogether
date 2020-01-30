using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PlayTogether.Models
{
    [ModelMetadataType(typeof(UsersMetadata))]
    public partial class Users
    {
    }

    [ModelMetadataType(typeof(PlayersMetadata))]
    public partial class Players
    {
    }

    [ModelMetadataType(typeof(GamesMetadata))]
    public partial class Games
    {
    }

    [ModelMetadataType(typeof(PlaceMetadata))]
    public partial class Places
    {
    }

    [ModelMetadataType(typeof(PasswordsConfirmationMetadata))]
    public partial class PasswordConfirmation
    {

    }
}