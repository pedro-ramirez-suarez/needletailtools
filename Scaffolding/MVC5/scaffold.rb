begin
	require 'nokogiri'
	Dir[File.dirname(__FILE__) + '/templates/*.rb'].each {|file| require file }
rescue LoadError
	puts "============ note ============="
	puts "Looks like you don't have nokogiri installed. Nokogiri is used to help you quickly scaffold models, view and controllers:" 
	puts "You *DO NOT* need scaffolding for the Oak interactive tutorial, or any development (just a nice to have)"
	puts "So don't worry about this note if you don't want scaffolding"
	puts "Instructions for setting up nokogiri (one time):"
	puts "  - Install chocolatey by running the powershell script located at chocolatey.org" 
	puts "  - After chocolatey is installed, run the command: cinst ruby.devkit (if you haven't installed ruby's DevKit)" 
	puts "  - Then run the command 'gem install nokogiri'"
	puts "  - Type 'rake -D gen' for more information on scaffolding (the source located in scaffold.rb)."
	puts "================================"
	puts ""
end

namespace :gen do

	desc ""
	task :controller, [:model] => :rake_dot_net_initialize do |t, args|
		raise "name parameter required, example: rake gen:controller[User]" if args[:model].nil?
		model_name = args[:model]
		file_name = model_name.ext("xml")

		#exec("XmlGenerator/Generator.exe")

		verify_file_name file_name

 		xml_file = File.open(file_name)
 		nkg_xml_model = Nokogiri::XML(xml_file)
		
 		@is_view_model = nkg_xml_model.xpath("//entity").length > 1

 		main_model = nkg_xml_model.xpath("//entity").first
		name = main_model['name']
 		primaryKeyType = main_model['primaryKeyType']
		create_controller_template main_model, primaryKeyType
	end



	desc "adds a CRUD scaffold, example: rake gen:crudFor[Entity]"
	task :crudFor, [:path] => :rake_dot_net_initialize do |t, args|
		raise "name parameter required, example: rake gen:crudFor[User]" if args[:path].nil?
		model_name = args[:path]
		file_name = model_name.ext("xml")
		file_path = "XmlGenerator/#{file_name}"

		verify_file_name file_name

		system("XmlGenerator/Generator.exe Views #{model_name} #{@mvc_project_directory}")

		xml_file = File.open(file_name)

		raise "File #{file_name} not found!!." if xml_file.nil? 

 		nkg_xml_model = Nokogiri::XML(xml_file)
		
 		@is_view_model = nkg_xml_model.xpath("//entity").length > 1

 		main_model = nkg_xml_model.xpath("//entity").first
		name = main_model['name']
 		primaryKeyType = main_model['primaryKeyType']

		create_controller_template main_model, primaryKeyType

		create_views_templates main_model

  		create_js_templates main_model

		create_repository_template name, primaryKeyType

		create_tests_controller_template main_model

		xml_file.close

		puts "Process completed!!"
	end

	desc "adds javascript file to your mvc project, example: rake gen:script[index]"
	task :script, [:name] => :rake_dot_net_initialize do |t, args|
		raise "js name required, example: rake gen:script[index]" if args[:name].nil?

		verify_file_name args[:name]

		folder "Scripts/app"

		save js_template(args[:name]), "#{@mvc_project_directory}/Scripts/app/#{args[:name]}.js"

		add_js_node args[:name]
	end

	def save content, file_path
		write_file = false
		if !File.exists?(file_path) || @overwrite_files
			write_file = true
		elsif @overwrite_files == nil 
			puts "Some files already exists, do you want replace all? (Y/N)" 
			@overwrite_files = (STDIN.gets.chomp == 'y')
			write_file = @overwrite_files
		end

		if write_file
			File.open(file_path, "w+") { |f| f.write(content) }
			puts "#{file_path} added"
		else
			puts "#{file_path} skipped"
		end
	end

	def folder dir
		FileUtils.mkdir_p "./#{@mvc_project_directory}/#{dir}/"
	end

	def add_compile_node folder, name, project = nil
		to_open = project || proj_file
		doc = Nokogiri::XML(open(to_open))
		if folder == :root
			add_include doc, :Compile, "#{name}.cs"
		else
			add_include doc, :Compile, "#{folder.to_s}\\#{name}.cs"
		end
		File.open(to_open, "w") { |f| f.write(doc) }
	end

	def add_cshtml_node folder, name
		doc = Nokogiri::XML(open(proj_file))
		add_include doc, :Content, "Views\\#{folder}\\#{name}.cshtml"
		File.open(proj_file, "w") { |f| f.write(doc) }
	end
	
	def add_js_node name
		doc = Nokogiri::XML(open(proj_file))
		add_include doc, :Content, "Scripts\\app\\#{name}.js"
		File.open(proj_file, "w") { |f| f.write(doc) }
	end

	def add_include doc, type, value
		if doc.xpath("//xmlns:#{type.to_s}[@Include='#{value}']").length == 0
			doc.xpath("//xmlns:ItemGroup[xmlns:#{type.to_s}]").first << "<#{type.to_s} Include=\"#{value}\" />"
		end
	end

	def proj_file
		"#{@mvc_project_directory}/#{@mvc_project_directory}.csproj"
	end

	def proj_tests_file
		"#{@test_project}/#{@test_project}.csproj"
	end

	def webconfig_file
		"#{@mvc_project_directory}/Web.config"
	end

	def verify_file_name name
		raise "You cant use #{name} as the name. No spaces or fancy characters please." if name =~ /[\x00\/\\:\*\?\"<>\|]/ || name =~ / /
	end

	def add_db_connection_string
		doc = Nokogiri::XML(open(webconfig_file))
		doc.xpath("//connectionStrings").first << "<add name='Default' providerName='System.Data.SqlClient' connectionString='Data Source=localhost\\SQLEXPRESS;Initial Catalog=#{@database_name};Persist Security Info=true;User ID=user_name;Password=Password1234' />"
		File.open(webconfig_file, "w") { |f| f.write(doc) }
	end

	def create_repository_template name, keytype
		folder "Repositories"
		repository_name = name + "Repository"
		save repository_template(name, keytype), "#{@mvc_project_directory}/Repositories/#{repository_name}.cs"
		add_compile_node :Repositories, repository_name
	end

	def create_model_template model
		model_name = model['name']
		save model_template(model), "#{@mvc_project_directory}/Models/#{model_name}.cs"
		add_compile_node :Models, model_name
	end

	def create_controller_template model, keytype
		controller_name = model['name'] + "Controller"
		save controller_template(model, keytype), "#{@mvc_project_directory}/Controllers/#{controller_name}.cs"
		add_compile_node :Controllers, controller_name
	end
	
	def create_views_templates model
		name = model['name']
		folder "Views/Shared"
		folder "Views/#{name}"

		save view_shared_layout_template(name), "#{@mvc_project_directory}/Views/Shared/_Layout.cshtml"
		add_cshtml_node "Shared", "_Layout"

		save view_index_template(model), "#{@mvc_project_directory}/Views/#{name}/Index.cshtml"
		add_cshtml_node name, "Index"

		save view_create_template(model), "#{@mvc_project_directory}/Views/#{name}/Create.cshtml"
		add_cshtml_node name, "Create"

		save view_details_template(model), "#{@mvc_project_directory}/Views/#{name}/Details.cshtml"
		add_cshtml_node name, "Details"

		save view_edit_template(model), "#{@mvc_project_directory}/Views/#{name}/Edit.cshtml"
		add_cshtml_node name, "Edit"
	end

	def create_js_templates model
		name = model['name']
		folder "Scripts/app"

		save js_binding_template(model), "#{@mvc_project_directory}/Scripts/app/#{name}.binding.js"
		add_js_node "#{name}.binding"

		save js_model_validate_template(model), "#{@mvc_project_directory}/Scripts/app/#{name}.validate.js"
		add_js_node "#{name}.validate"

		save js_require_config_template, "#{@mvc_project_directory}/Scripts/app/require.config.js"
		add_js_node "require.config"
	end

	def create_db_context_templates name
		folder "Context"

		save context_template(name), "#{@mvc_project_directory}/Context/#{name}Context.cs"
		add_compile_node :Context, "#{name}Context"	
		#add_db_connection_string
	end

	def create_tests_controller_template model
		name = model['name']
		file_name = "#{name}Controller_spec"
		save tests_controller_template(model), "#{@test_project}/#{file_name}.cs"
		add_compile_node :root, file_name, proj_tests_file
	end
end

