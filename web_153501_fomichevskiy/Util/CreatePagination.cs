using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System.Web;

namespace web_153501_fomichevskiy.Util;

public static class PaginationHelper
{
	/*<nav class="col-sm-4 offset-2">
				<ul class="pagination">
					<li class="page-item @(previousAvailable ? "" : "disabled")">
						<a class="page-link" aria-label="Previous"
						   asp-controller="Product"
						   asp-action="Index"
						   asp-route-category=@category
						   
						   asp-route-pageNo=@(Model.currentPage-1)>		
							<span aria-hidden="true">&laquo;</span>
						</a> <!-- asp-route-currentCategory=currentCategory -->
					</li>
					@for (int i = first_p; i <= last_p; ++i)
					{
						<li class="page-item @(Model.currentPage == i ? "active" : "")">
							<a class="page-link"
							   asp-controller="Product"
							   asp-action="Index"
							   asp-route-category=@category
							   
							   asp-route-pageNo="@i">@i</a>
						</li> <!-- asp-route-currentCategory=currentCategory -->
					}
					<li class="page-item @(nextAvailable ? "" : "disabled")">
						<a class="page-link" aria-label="Next"
						   asp-controller="Product"
						   asp-action="Index"
						   asp-route-category=@category
						   
						   asp-route-pageNo=@(Model.currentPage+1)>
							<span aria-hidden="true">&raquo;</span>
						</a> <!-- asp-route-currentCategory=currentCategory -->
					</li>
				</ul>
			</nav>*/
	public static HtmlString CreatePagination(this IHtmlHelper hmtl, int currentPage, int totalPages, string? category)
	{
		int first_p;
		int last_p;

		if (totalPages <= 2)
		{
			first_p = 1;
			last_p = totalPages;
		}
		else
		{
			if (currentPage == 1)
			{
				first_p = 1;
				last_p = 3;
			}
			else if (currentPage == totalPages)
			{
				first_p = totalPages - 2;
				last_p = totalPages;
			}
			else
			{
				first_p = currentPage - 1;
				last_p = currentPage + 1;
			}
		}



		bool previousAvailable = currentPage != 1;
		bool nextAvailable = currentPage != totalPages;

		TagBuilder nav = new TagBuilder("nav");
		nav.Attributes.Add("class", "col-sm-4 offset-2");

		TagBuilder ul = new TagBuilder("ul");
		ul.Attributes.Add("class", "pagination");

		//prev btn
		TagBuilder prev_li = new TagBuilder("li");

		if (previousAvailable)
		{
			prev_li.Attributes.Add("class", "page-item");
		}
		else
		{
			prev_li.Attributes.Add("class", "page-item disabled");
		}

		TagBuilder a = new TagBuilder("a");
		a.Attributes.Add("class", "page-link");
		a.Attributes.Add("aria-label", "Previous");
		a.Attributes.Add("asp-controller", "Product");
		a.Attributes.Add("asp-action", "Index");
		a.Attributes.Add("asp-route-pageNo", $"{currentPage-1}");
		a.Attributes.Add("asp-route-category", category);

		TagBuilder prev_span = new TagBuilder("span");
		prev_span.Attributes.Add("aria-hidden", "true");
		prev_span.InnerHtml.Append("&laquo;");

		a.InnerHtml.AppendHtml(prev_span);

		prev_li.InnerHtml.AppendHtml(a);

		ul.InnerHtml.AppendHtml(prev_li);

		//num btns
		for (int i = first_p; i<= last_p; i++)
		{
			TagBuilder num_li = new TagBuilder("li");

			if (i == currentPage)
			{
				num_li.AddCssClass("page-item active");
			}
			else
			{
				num_li.AddCssClass("page-item");
			}

			TagBuilder num_a = new TagBuilder("a");
			num_a.MergeAttribute("class", "page-link");
			num_a.MergeAttribute("asp-controller", "Product");
			num_a.MergeAttribute("action", "Index");
			num_a.MergeAttribute("asp-route-category", category);
			num_a.MergeAttribute("asp-route-pageNo", $"{i}");
			num_a.InnerHtml.Append($"{i}");

			num_li.InnerHtml.AppendHtml(num_a);

			ul.InnerHtml.AppendHtml(num_li);
		}

		//next_btn
		TagBuilder next_li = new TagBuilder("li");

		if (nextAvailable)
		{
			next_li.Attributes.Add("class", "page-item");
		}
		else
		{
			next_li.Attributes.Add("class", "page-item disabled");
		}

		TagBuilder next_a = new TagBuilder("a");
		next_a.Attributes.Add("class", "page-link");
		next_a.Attributes.Add("aria-label", "Next");
		next_a.Attributes.Add("asp-controller", "Product");
		next_a.Attributes.Add("asp-action", "Index");
		next_a.Attributes.Add("asp-route-pageNo", $"{currentPage + 1}");
		next_a.Attributes.Add("asp-route-category", category);

		TagBuilder next_span = new TagBuilder("span");
		next_span.Attributes.Add("aria-hidden", "true");
		next_span.InnerHtml.Append("&raquo;");

		next_a.InnerHtml.AppendHtml(next_span);

		ul.InnerHtml.AppendHtml(next_li);

		nav.InnerHtml.AppendHtml(ul); 

		var writer = new System.IO.StringWriter();
		nav.WriteTo(writer, HtmlEncoder.Default);
		//var tnp = nav.ToString(TagRenderMode.Normal);
		return new HtmlString(writer.ToString());
	}
}
