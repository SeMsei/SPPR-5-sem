using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace web_153501_fomichevskiy.TagHelpers;

[HtmlTargetElement("pager")]
public class Pager : TagHelper
{
	private readonly LinkGenerator _linkGenerator;
	private readonly HttpContext _httpContext;

	public Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
	{
		_linkGenerator = linkGenerator;
		_httpContext = httpContextAccessor.HttpContext!;
	}

	[HtmlAttributeName("current-page")]
	public int CurrentPage { get; set; }

	[HtmlAttributeName("total-pages")]
	public int TotalPages { get; set; }

	[HtmlAttributeName("category")]
	public string? Category { get; set; }

	[HtmlAttributeName("current-category")]
	public string? CurrentCategory { get; set; }


	[HtmlAttributeName("is-admin")]
	public bool Admin { get; set; }


	private RouteValueDictionary GetRouteValues(int pageNo)
	{
		RouteValueDictionary values = null!;
		if (Admin)
		{
			values = new RouteValueDictionary
				{
					{ "pageNo", pageNo },
				};
		}
		else
		{
			values = new RouteValueDictionary
				{
					{ "category", Category },
					{ "currentCategory", CurrentCategory },
					{ "pageNo", pageNo }
				};
		}
		return values;
	}

	private string? GetUrl(int pageNo)
	{
		return _linkGenerator.GetPathByPage(_httpContext, values: GetRouteValues(pageNo));
	}

	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = "nav";
		output.Attributes.SetAttribute("class", "col-sm-4 offset-2");

		var ulTag = new TagBuilder("ul");
		ulTag.AddCssClass("pagination");

		var previousAvailable = CurrentPage > 1;
		var nextAvailable = CurrentPage < TotalPages;

		var previousLiTag = new TagBuilder("li");
		previousLiTag.AddCssClass(previousAvailable ? "page-item" : "page-item disabled");

		var previousLink = new TagBuilder("a");
		previousLink.AddCssClass("page-link");
		previousLink.Attributes["aria-label"] = "Previous";

		if (previousAvailable)
		{
			var previousUrl = GetUrl(CurrentPage - 1);
			previousLink.Attributes["href"] = previousUrl;
		}

		var previousSpan = new TagBuilder("span");
		previousSpan.InnerHtml.Append("\u00AB");

		previousLink.InnerHtml.AppendHtml(previousSpan);
		previousLiTag.InnerHtml.AppendHtml(previousLink);
		ulTag.InnerHtml.AppendHtml(previousLiTag);

		for (int i = 1; i <= TotalPages; i++)
		{
			var liTag = new TagBuilder("li");
			liTag.AddCssClass("page-item");
			if (CurrentPage == i)
			{
				liTag.AddCssClass("active");
			}

			var link = new TagBuilder("a");
			link.AddCssClass("page-link");

			var url = GetUrl(i);
			link.Attributes["href"] = url;
			link.InnerHtml.Append(i.ToString());

			liTag.InnerHtml.AppendHtml(link);
			ulTag.InnerHtml.AppendHtml(liTag);
		}

		var nextLiTag = new TagBuilder("li");
		nextLiTag.AddCssClass(nextAvailable ? "page-item" : "page-item disabled");

		var nextLink = new TagBuilder("a");
		nextLink.AddCssClass("page-link");
		nextLink.Attributes["aria-label"] = "Next";

		if (nextAvailable)
		{
			var nextUrl = GetUrl(CurrentPage + 1);
			nextLink.Attributes["href"] = nextUrl;
		}

		var nextSpan = new TagBuilder("span");
		nextSpan.InnerHtml.Append("\u00BB");

		nextLink.InnerHtml.AppendHtml(nextSpan);
		nextLiTag.InnerHtml.AppendHtml(nextLink);
		ulTag.InnerHtml.AppendHtml(nextLiTag);

		output.Content.AppendHtml(ulTag);
	}
}


/*using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace web_153501_fomichevskiy.TagHelpers;

[HtmlTargetElement("pager")]
public class Pager : TagHelper
{
	private LinkGenerator _linkGenerator;
	private IHttpContextAccessor _httpContextAccessor;

	public Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
	{
		_linkGenerator = linkGenerator;
		_httpContextAccessor = httpContextAccessor;
	}

	[HtmlAttributeName("current-page")]
	public int CurrentPage { get; set; }

	[HtmlAttributeName("total-pages")]
	public int TotalPages { get; set; }

	[HtmlAttributeName("is-admin")]
	public bool IsAdmin { get; set; }

	public override void Process(TagHelperContext context, TagHelperOutput output)
	{

		TagBuilder result = new TagBuilder("ul");
		result.AddCssClass("pagination justify-content-center");

		var prev = CurrentPage == 1 ? 1 : CurrentPage - 1;
		var next = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

		var prevUrlAdmin = _linkGenerator.GetPathByPage(_httpContextAccessor.HttpContext, page: "/Index", values: new { area = "Admin", pageNo = prev });
		var prevUrl = IsAdmin ? prevUrlAdmin : _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "Index", "Product", new { pageNo = prev });

		var liPrev = new TagBuilder("li");
		liPrev.AddCssClass(CurrentPage == 1 ? "page-item disabled" : "page-item");
		var aPrev = new TagBuilder("a");
		aPrev.AddCssClass("page-link");
		aPrev.MergeAttribute("href", prevUrl);
		aPrev.InnerHtml.Append("«");
		liPrev.InnerHtml.AppendHtml(aPrev);
		result.InnerHtml.AppendHtml(liPrev);

		/*int first_p;
		int last_p;

		if (TotalPages <= 2)
		{
			first_p = 1;
			last_p = TotalPages;
		}
		else
		{
			if (CurrentPage == 1)
			{
				first_p = 1;
				last_p = 3;
			}
			else if (CurrentPage == TotalPages)
			{
				first_p = TotalPages - 2;
				last_p = TotalPages;
			}
			else
			{
				first_p = CurrentPage - 1;
				last_p = CurrentPage + 1;
			}
		}

		for (int i = 1; i <= TotalPages; i++)
		{
			var li = new TagBuilder("li");
			li.AddCssClass(i == CurrentPage ? "page-item active" : "page-item");
			var a = new TagBuilder("a");
			a.AddCssClass("page-link");

			var urlAdmin = _linkGenerator.GetPathByPage(_httpContextAccessor.HttpContext, page: "/Index", values: new { area = "Admin", pageNo = i });
			var url = IsAdmin ? urlAdmin : _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "Index", "Product", new { pageNo = i });

			a.MergeAttribute("href", url);
			a.InnerHtml.Append(i.ToString());
			li.InnerHtml.AppendHtml(a);
			result.InnerHtml.AppendHtml(li);
		}

		var liNext = new TagBuilder("li");
		liNext.AddCssClass(CurrentPage == TotalPages ? "page-item disabled" : "page-item");
		var aNext = new TagBuilder("a");
		aNext.AddCssClass("page-link");

		var nextUrlAdmin = _linkGenerator.GetPathByPage(_httpContextAccessor.HttpContext, page: "/Index", values: new { area = "Admin", pageNo = next });
		var nexrUrl = IsAdmin ? nextUrlAdmin : _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "Index", "Product", new { pageNo = next });

		aNext.MergeAttribute("href", nexrUrl);
		aNext.InnerHtml.Append("»");
		liNext.InnerHtml.AppendHtml(aNext);
		result.InnerHtml.AppendHtml(liNext);

		output.Content.AppendHtml(result);
	}
}


/*using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace web_153501_fomichevskiy.TagHelpers;

[HtmlTargetElement("pager")]
public class Pager : TagHelper
{
private readonly LinkGenerator _linkGenerator;
private readonly HttpContext _httpContext;

public Pager(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
{
	_linkGenerator = linkGenerator;
	_httpContext = httpContextAccessor.HttpContext!;
}

[HtmlAttributeName("current-page")]
public int CurrentPage { get; set; }

[HtmlAttributeName("total-pages")]
public int TotalPages { get; set; }

[HtmlAttributeName("category")]
public string? Category { get; set; }

[HtmlAttributeName("current-category")]
public string? CurrentCategory { get; set; }


[HtmlAttributeName("admin")]
public bool Admin { get; set; }


private RouteValueDictionary GetRouteValues(int pageNo)
{
	RouteValueDictionary values = null!;
	if (Admin)
	{
		values = new RouteValueDictionary
			{
				{ "pageNo", pageNo },
			};
	}
	else
	{
		values = new RouteValueDictionary
			{
				{ "category", Category },
				{ "currentCategory", CurrentCategory },
				{ "pageNo", pageNo }
			};
	}
	return values;
}

private string? GetUrl(int pageNo)
{
	return _linkGenerator.GetPathByPage(_httpContext, values: GetRouteValues(pageNo));
}

public override void Process(TagHelperContext context, TagHelperOutput output)
{
	output.TagName = "nav";
	output.Attributes.SetAttribute("class", "col-sm-4 offset-2");

	var ulTag = new TagBuilder("ul");
	ulTag.AddCssClass("pagination");

	var previousAvailable = CurrentPage > 1;
	var nextAvailable = CurrentPage < TotalPages;

	var previousLiTag = new TagBuilder("li");
	previousLiTag.AddCssClass(previousAvailable ? "page-item" : "page-item disabled");

	var previousLink = new TagBuilder("a");
	previousLink.AddCssClass("page-link");
	previousLink.Attributes["aria-label"] = "Previous";

	if (previousAvailable)
	{
		var previousUrl = GetUrl(CurrentPage - 1);
		previousLink.Attributes["href"] = previousUrl;
	}

	var previousSpan = new TagBuilder("span");
	previousSpan.InnerHtml.Append("\u00AB");

	previousLink.InnerHtml.AppendHtml(previousSpan);
	previousLiTag.InnerHtml.AppendHtml(previousLink);
	ulTag.InnerHtml.AppendHtml(previousLiTag);

	int first_p;
	int last_p;

	if (TotalPages <= 2)
	{
		first_p = 1;
		last_p = TotalPages;
	}
	else
	{
		if (CurrentPage == 1)
		{
			first_p = 1;
			last_p = 3;
		}
		else if (CurrentPage == TotalPages)
		{
			first_p = TotalPages - 2;
			last_p = TotalPages;
		}
		else
		{
			first_p = CurrentPage - 1;
			last_p = CurrentPage + 1;
		}
	}

	for (int i = first_p; i <= last_p; i++)
	{
		var liTag = new TagBuilder("li");
		liTag.AddCssClass("page-item");
		if (CurrentPage == i)
		{
			liTag.AddCssClass("active");
		}

		var link = new TagBuilder("a");
		link.AddCssClass("page-link");

		var url = GetUrl(i);
		link.Attributes["href"] = url;
		link.InnerHtml.Append(i.ToString());

		liTag.InnerHtml.AppendHtml(link);
		ulTag.InnerHtml.AppendHtml(liTag);
	}

	var nextLiTag = new TagBuilder("li");
	nextLiTag.AddCssClass(nextAvailable ? "page-item" : "page-item disabled");

	var nextLink = new TagBuilder("a");
	nextLink.AddCssClass("page-link");
	nextLink.Attributes["aria-label"] = "Next";

	if (nextAvailable)
	{
		var nextUrl = GetUrl(CurrentPage + 1);
		nextLink.Attributes["href"] = nextUrl;
	}

	var nextSpan = new TagBuilder("span");
	nextSpan.InnerHtml.Append("\u00BB");

	nextLink.InnerHtml.AppendHtml(nextSpan);
	nextLiTag.InnerHtml.AppendHtml(nextLink);
	ulTag.InnerHtml.AppendHtml(nextLiTag);

	output.Content.AppendHtml(ulTag);
}
}*/