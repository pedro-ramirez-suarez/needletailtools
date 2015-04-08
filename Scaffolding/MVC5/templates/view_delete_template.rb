def view_delete_template name
return <<template
@{
    ViewBag.Title = "Delete";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<fieldset>
    <legend>User</legend>

    <div class="display-label">
         @Html.DisplayNameFor(model => model.Name)
    </div>
    <div class="display-field">
        @Html.DisplayFor(model => model.Name)
    </div>

    <div class="display-label">
         @Html.DisplayNameFor(model => model.Email)
    </div>
    <div class="display-field">
        @Html.DisplayFor(model => model.Email)
    </div>

    <div class="display-label">
         @Html.DisplayNameFor(model => model.City)
    </div>
    <div class="display-field">
        @Html.DisplayFor(model => model.City)
    </div>
</fieldset>
@using (Html.BeginForm()) {
    @Html.AntiForgeryToken()
    <p>
        <input type="submit" value="Delete" /> |
        @Html.ActionLink("Back to List", "Index")
    </p>
}

template
end